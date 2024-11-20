import { z } from 'zod'
import { deviceSchema } from './device'

export const deviceLogSchema = z.object({
  level: z.enum(['error', 'warning', 'info']),
  message: z.string(),
  timestamp: z.coerce.date().max(new Date()),
  device: deviceSchema,
})

export type DeviceLog = z.infer<typeof deviceLogSchema>
