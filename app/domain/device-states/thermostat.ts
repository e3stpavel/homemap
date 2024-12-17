import { z } from 'zod'
import { baseDeviceStateSchema } from './base'

export const thermostatDeviceStateSchema = baseDeviceStateSchema.merge(
  z.object({
    $type: z.literal('thermostat'),
    temperature: z.number().min(18).max(30),
  }),
)
