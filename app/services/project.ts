import { z } from 'zod'
import { deviceLogSchema } from '~/domain/device-log'
import { updateProjectSchema, projectSchema, type Project, createProjectSchema } from '~/domain/project'

export const useProjectService = () => {
  const config = useRuntimeConfig()
  const baseUrl = `${config.public.apiBaseUrl}/projects`

  const repository = createProjectRepository(
    $fetch.create({
      baseURL: baseUrl,
    }),
  )

  return {
    async getProjects() {
      const response = await repository.findAll()
      return z.array(projectSchema).parse(response)
    },

    async getProjectById(id: Project['id']) {
      const response = await repository.findOne(id)
      return projectSchema.parse(response)
    },

    async *streamDeviceLogsById(id: Project['id'], signal: AbortSignal) {
      const eventStream = createEventStream(`${baseUrl}/${id}/logs/stream`, {
        signal,
      })

      for await (const event of eventStream) {
        const validationResult = deviceLogSchema.safeParse(event.message)

        if (validationResult.success)
          yield validationResult.data
      }
    },

    async removeProjectById(id: Project['id']) {
      await repository.remove(id)
    },

    async updateProject(id: Project['id'], updatedProject: Record<string, unknown>) {
      const project = updateProjectSchema.parse({ id, ...updatedProject })

      const response = await repository.update(id, project)
      return projectSchema.parse(response)
    },

    async createProject(newProject: Record<string, unknown>) {
      const project = createProjectSchema.parse(newProject)

      const response = await repository.add(project)
      return projectSchema.parse(response)
    },
  }
}
