import type { Device } from '~/domain/device'
import { deviceStateSchema } from '~/domain/device-state'

export const useDeviceStateService = () => {
  const config = useRuntimeConfig()
  const repository = createDeviceRepository(
    $fetch.create({
      baseURL: `${config.public.apiBaseUrl}/devices`,
    }),
  )

  return {
    async getDeviceState(deviceId: Device['id'], abortSignal: AbortSignal) {
      const response = await repository.pullState(deviceId, abortSignal)

      return deviceStateSchema.parse(response)
    },

    async setDeviceState(deviceId: Device['id'], newDeviceState: Record<string, unknown>) {
      const deviceState = deviceStateSchema.parse(newDeviceState)

      await repository.pushState(deviceId, deviceState)
    },
  }
}
