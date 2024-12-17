import { z } from 'zod'

export const baseDeviceStateSchema = z.object({
  isTurnedOn: z.boolean(),
})
