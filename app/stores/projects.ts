// https://github.com/nuxt/nuxt/issues/28804
import { useRoute } from 'vue-router'
import type { DeviceLog } from '~/domain/device-log'
import type { Project } from '~/domain/project'
import { useProjectService } from '~/services/project'

export const useProjectsStore = defineStore('projects', () => {
  const route = useRoute()
  const projectService = useProjectService()

  const projects = ref<Project[]>([])

  async function getProjects() {
    projects.value = await projectService.getProjects()
    return projects.value
  }

  const currentProjectId = computed(() => {
    const id = route.params.id

    if (typeof id === 'string' && /^\d+$/.test(id))
      return +id
  })

  const currentProject = ref<Project>()

  async function getCurrentProject() {
    if (!currentProjectId.value)
      return { success: false }

    currentProject.value = await projectService.getProjectById(currentProjectId.value)
    return currentProject.value
  }

  const deviceLogs = reactive<DeviceLog[]>([])

  async function streamDeviceLogs(signal: AbortSignal) {
    if (!currentProjectId.value)
      return

    const logs = projectService.streamDeviceLogsById(currentProjectId.value, signal)

    for (const log of logs) {
      try {
        const deviceLog = await log.read()
        deviceLogs.unshift(deviceLog)
      }
      catch (error) {
        if (!signal.aborted) {
          if (error instanceof Error)
            console.error(error)
        }
      }
    }

    // just for demo cleans all the logs here
    deviceLogs.splice(0, deviceLogs.length)
  }

  async function removeProject(projectId: Project['id']) {
    await projectService.removeProjectById(projectId)

    const index = projects.value.findIndex(project => project.id === projectId)
    projects.value.splice(index, 1)
  }

  async function updateCurrentProject(updatedProject: Record<string, unknown>) {
    if (!currentProjectId.value)
      return

    currentProject.value = await projectService.updateProject(currentProjectId.value, updatedProject)
  }

  async function createProject(newProject: Record<string, unknown>) {
    const createdProject = await projectService.createProject(newProject)
    projects.value.push(createdProject)

    return createdProject
  }

  return {
    projects,
    getProjects,
    createProject,
    updateCurrentProject,
    currentProject,
    currentProjectId,
    getCurrentProject,
    removeProject,
    deviceLogs,
    streamDeviceLogs,
  }
})
