<script setup lang="ts">
import type { DeviceLog } from '~/domain/device-log'

// const projectsStore = useProjectsStore()
// const { deviceLogs } = storeToRefs(projectsStore)

// const stream = projectsStore.streamDeviceLogs()
// stream?.startStreaming()

// // when user exits the page
// onBeforeUnmount(() => stream?.stopStreaming())

// TODO: make better
const dtf = new Intl.DateTimeFormat('en', {
  timeStyle: 'medium',
  dateStyle: 'medium',
  hour12: false,
})

const deviceLogs: DeviceLog[] = [
  { level: 'error', message: 'hello', timestamp: new Date(), device: { id: 1, name: 'testing' } },
  { level: 'warning', message: 'hello lorem ekeepo jekepep jeowpwp mwlw wl', timestamp: new Date(), device: { id: 1, name: 'testing' } },
  { level: 'info', message: 'hello bruh', timestamp: new Date(), device: { id: 1, name: 'testing' } },
  { level: 'error', message: 'hello', timestamp: new Date(), device: { id: 1, name: 'testing' } },
  { level: 'error', message: 'hello', timestamp: new Date(), device: { id: 1, name: 'testing' } },
  { level: 'info', message: 'hello bruh', timestamp: new Date(), device: { id: 1, name: 'testing' } },
]
</script>

<template>
  <div>
    <div class="max-w-3xl">
      <h1 class="text-3xl font-semibold tracking-tight">
        Event logs
      </h1>
      <p class="mt-2 leading-7">
        Monitor and track all device logs within your project
      </p>
    </div>

    <div class="mt-6 w-full">
      <Table>
        <TableHead>
          <TableRow>
            <TableHeader label="Type" />
            <TableHeader label="Device" />
            <TableHeader label="Message" />
            <TableHeader label="Timestamp" />
          </TableRow>
        </TableHead>
        <TableBody>
          <TableRow
            v-for="deviceLog in deviceLogs"
            :key="`${deviceLog.level}-${deviceLog.device.id}-${deviceLog.timestamp}`"
          >
            <TableData>
              <Badge
                :label="deviceLog.level"
                :variant="deviceLog.level"
                is-dot
              />
            </TableData>
            <TableData>
              <NuxtLink
                to="#"
                class="text-blue-600 font-medium group-hover/row:(text-blue-500 underline underline-offset-1)"
              >
                <span>
                  <span class="sr-only">Open device</span>
                  {{ deviceLog.device.name }}
                  <span class="sr-only">logs</span>
                </span>
                <span
                  class="absolute inset-0"
                  aria-hidden="true"
                />
              </NuxtLink>
            </TableData>
            <TableData>
              <span class="font-mono">
                {{ deviceLog.message }}
              </span>
            </TableData>
            <TableData>
              <span class="text-zinc-600 tabular-nums group-hover/row:text-zinc-900">
                {{ dtf.format(deviceLog.timestamp) }}
              </span>
            </TableData>
          </TableRow>
        </TableBody>
      </Table>
    </div>
  </div>
</template>
