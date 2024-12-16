import type { DeviceState } from '~/domain/device-state'
import { useDeviceStateService } from '~/services/device-state'

export const useDeviceStateStore = defineStore('device-state', () => {
  const devicesStore = useDevicesStore()
  const deviceStateService = useDeviceStateService()

  const { currentDeviceId } = storeToRefs(devicesStore)
  const currentDeviceState = ref<DeviceState>()

  async function getCurrentDeviceState(abortSignal: AbortSignal) {
    if (!currentDeviceId.value) {
      // reset state if device was de-selected
      // currentDeviceState.value = undefined
      return
    }

    currentDeviceState.value = await deviceStateService.getDeviceState(currentDeviceId.value, abortSignal)
    return currentDeviceState.value
  }

  async function updateCurrentDeviceState(updatedDeviceState: Record<string, unknown>) {
    if (!currentDeviceId.value)
      return

    await deviceStateService.setDeviceState(currentDeviceId.value, updatedDeviceState)
  }

  return {
    currentDeviceState,
    getCurrentDeviceState,
    updateCurrentDeviceState,
  }
})
