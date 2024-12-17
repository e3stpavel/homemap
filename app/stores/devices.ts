import type { Device } from '~/domain/device'
import type { Receiver } from '~/domain/receiver'
import { useDeviceService } from '~/services/device'

export const useDevicesStore = defineStore('devices', () => {
  const deviceService = useDeviceService()

  const devices: Record<Receiver['id'], Device[] | undefined> = reactive({})

  async function getDevicesByReceiver(receiverId: Receiver['id']) {
    if (receiverId && !devices[receiverId]) {
      devices[receiverId] = await deviceService.getDevices(receiverId)
      return devices[receiverId]
    }

    return { success: false }
  }

  const currentDeviceId = ref<Device['id']>()

  function setCurrentDeviceId(deviceId: Device['id']) {
    currentDeviceId.value = deviceId
  }

  function unsetCurrentDeviceId() {
    currentDeviceId.value = undefined
  }

  return {
    devices,
    getDevicesByReceiver,

    currentDeviceId,
    setCurrentDeviceId,
    unsetCurrentDeviceId,
  }
})
