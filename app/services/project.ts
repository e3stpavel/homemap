import { z } from 'zod'
import { deviceLogSchema, type DeviceLog } from '~/domain/device-log'
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

    streamDeviceLogsById(id: Project['id']) {
      const deviceLog = ref<DeviceLog>()
      const { data, open, close } = useEventSource(`${baseUrl}/${id}/logs/stream`, [], {
        immediate: false,
      })

      watchEffect(() => {
        const validationResult = deviceLogSchema.safeParse(JSON.parse(data.value ?? ''))
        if (!validationResult.success)
          return

        deviceLog.value = validationResult.data
      })

      return { deviceLog, startStreaming: open, stopStreaming: close }
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
