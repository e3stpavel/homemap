<script setup lang="ts">
const abortController = new AbortController()

// abort when user exits the page
onBeforeUnmount(() => abortController.abort())

const projectStore = useProjectsStore()

// start streaming on the client (fire and forget)
//  once component is mounted to avoid hydration mismatches
onMounted(() => projectStore.streamDeviceLogs(abortController.signal))

const dtf = new Intl.DateTimeFormat('en', {
  timeStyle: 'medium',
  dateStyle: 'medium',
  hour12: false,
})

// TODO: preserve position when neẇ items added
const { list, containerProps, wrapperProps } = useVirtualList(projectStore.deviceLogs, {
  itemHeight: 40,
})
</script>

<template>
  <div
    class="h-screen-sm overflow-auto whitespace-nowrap -mx-6"
    v-bind="containerProps"
  >
    <div class="inline-block min-w-full sm:px-6">
      <table
        v-if="projectStore.deviceLogs.length > 0"
        class="text-sm"
        v-bind="wrapperProps"
      >
        <thead class="sticky top-0 border-b bg-white text-zinc-600 shadow-sm">
          <DeviceLogsTableRow variant="head">
            <DeviceLogsTableCell variant="head">
              Type
            </DeviceLogsTableCell>
            <DeviceLogsTableCell variant="head">
              Device
            </DeviceLogsTableCell>
            <DeviceLogsTableCell variant="head">
              Message
            </DeviceLogsTableCell>
            <DeviceLogsTableCell variant="head">
              Timestamp
            </DeviceLogsTableCell>
          </DeviceLogsTableRow>
        </thead>
        <tbody class="divide-y">
          <DeviceLogsTableRow
            v-for="{ index, data: log } in list"
            :key="`${log.level}-${log.device.id}-${log.timestamp}-${index}`"
          >
            <DeviceLogsTableCell>
              <Badge
                :label="log.level"
                :variant="log.level"
                is-dot
              />
            </DeviceLogsTableCell>
            <DeviceLogsTableCell>
              <span class="truncate text-blue-600 font-medium">
                {{ log.device.name }}
              </span>
            </DeviceLogsTableCell>
            <DeviceLogsTableCell>
              <span class="truncate font-mono">
                {{ log.message }}
              </span>
            </DeviceLogsTableCell>
            <DeviceLogsTableCell>
              <span class="text-zinc-600 tabular-nums group-hover/row:text-zinc-900">
                {{ dtf.format(log.timestamp) }}
              </span>
            </DeviceLogsTableCell>
          </DeviceLogsTableRow>
        </tbody>
      </table>
      <div
        v-else
        class="mx-auto max-w-md px-4 py-8"
      >
        <div class="mx-auto size-16 flex items-center justify-center border rounded-lg text-3xl text-blue-600 shadow-sm sm:text-4xl">
          <Icon name="i-material-symbols-settings-input-antenna-rounded" />
        </div>
        <h3 class="mt-4 text-center font-semibold leading-6">
          Listening to logs of project devices
        </h3>
        <p class="mt-1 text-center text-sm text-zinc-600 leading-6">
          As soon as devices within the project start producing logs, they will appear here
        </p>
        <div class="mx-auto mt-6 w-fit">
          <NuxtLink
            to="#"
            class="min-w-fit inline-flex items-center justify-center gap-2 text-sm text-blue-600 font-medium hover:(text-blue-500 underline underline-offset-1)"
          >
            Read more about logs
            <Icon name="i-material-symbols-arrow-right-alt-rounded" />
          </NuxtLink>
        </div>
      </div>
    </div>
  </div>
</template>
