import { z } from 'zod'
import {
  acDeviceStateSchema,
  lightbulbDeviceStateSchema,
  socketDeviceStateSchema,
  thermostatDeviceStateSchema,
} from './device-states'

export const deviceStateSchema = z.discriminatedUnion('$type', [
  acDeviceStateSchema,
  lightbulbDeviceStateSchema,
  socketDeviceStateSchema,
  thermostatDeviceStateSchema,
])

export type DeviceState = z.infer<typeof deviceStateSchema>
