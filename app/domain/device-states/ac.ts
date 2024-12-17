import { z } from 'zod'
import { baseDeviceStateSchema } from './base'

export const acDeviceStateSchema = baseDeviceStateSchema.merge(
  z.object({
    $type: z.literal('ac'),
    temperature: z.number().min(18).max(30),
  }),
)
