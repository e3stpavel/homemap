import { z } from 'zod'
import { baseDeviceStateSchema } from './base'

export const socketDeviceStateSchema = baseDeviceStateSchema.merge(
  z.object({
    $type: z.literal('socket'),
  }),
)
