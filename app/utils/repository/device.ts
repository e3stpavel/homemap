import type { $Fetch } from 'nitropack'
import { createRepository } from './base'
import type { Receiver } from '~/domain/receiver'
import type { CreateDevice, Device } from '~/domain/device'
import type { DeviceState } from '~/domain/device-state'

export const createDeviceRepository = (fetch: $Fetch) => {
  const { findOne, update, remove } = createRepository(fetch)

  return {
    async findAll(receiverId: Receiver['id']) {
      return await fetch<unknown[]>(
        // to learn why this works
        //  https://github.com/unjs/ufo
        `../receivers/${receiverId}/devices`,
      )
    },

    async add(receiverId: Receiver['id'], newDevice: CreateDevice) {
      return await fetch<unknown>(
        `../receivers/${receiverId}/devices`,
        {
          method: 'POST',
          body: newDevice,
        },
      )
    },

    findOne,
    update,
    remove,

    async pullState(deviceId: Device['id'], signal: AbortSignal) {
      return await fetch<unknown>(`/${deviceId}/state`, {
        signal,
      })
    },

    async pushState(deviceId: Device['id'], newDeviceState: DeviceState) {
      return await fetch<unknown>(`/${deviceId}/state`, {
        method: 'POST',
        body: newDeviceState,
      })
    },
  }
}
