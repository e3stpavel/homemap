import { z } from 'zod'
import { baseDeviceStateSchema } from './base'

export const lightbulbDeviceStateSchema = baseDeviceStateSchema.merge(
  z.object({
    $type: z.literal('lightbulb'),
    lightTemperature: z.number().min(1500).max(7000),
    brightness: z.number().min(0).max(100),
  }),
)
