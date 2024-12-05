<script setup lang="ts">
const abortController = new AbortController()

// abort when user exits the page
onBeforeUnmount(() => abortController.abort())

const projectStore = useProjectsStore()
projectStore.streamDeviceLogs(abortController.signal)

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
        class="text-sm"
        v-bind="wrapperProps"
      >
        <thead class="sticky top-0 border-b bg-white text-zinc-600 shadow-sm">
          <tr class="grid auto-flow-col h-10 divide-x">
            <th class="w-32 flex items-center px-4 font-medium first:pl-6 last:pr-6 sm:(first:pl-1 last:pr-1)">
              Type
            </th>
            <th class="w-56 flex items-center px-4 font-medium first:pl-6 last:pr-6 sm:(first:pl-1 last:pr-1)">
              Device
            </th>
            <th class="w-screen-md flex items-center px-4 font-medium first:pl-6 last:pr-6 sm:(first:pl-1 last:pr-1)">
              Message
            </th>
            <th class="w-56 flex items-center px-4 font-medium first:pl-6 last:pr-6 sm:(first:pl-1 last:pr-1)">
              Timestamp
            </th>
          </tr>
        </thead>
        <tbody class="divide-y">
          <tr
            v-for="{ index, data: log } in list"
            :key="`${log.level}-${log.device.id}-${log.timestamp}-${index}`"
            class="grid auto-flow-col h-10 divide-x odd:bg-zinc-50"
          >
            <td class="w-32 flex items-center overflow-hidden px-4 first:pl-6 last:pr-6 sm:(first:pl-1 last:pr-1)">
              <Badge
                :label="log.level"
                :variant="log.level"
                is-dot
              />
            </td>
            <td class="w-56 flex items-center overflow-hidden px-4 first:pl-6 last:pr-6 sm:(first:pl-1 last:pr-1)">
              <span class="truncate text-blue-600 font-medium">
                {{ log.device.name }}
              </span>
            </td>
            <td class="w-screen-md flex items-center overflow-hidden px-4 first:pl-6 last:pr-6 sm:(first:pl-1 last:pr-1)">
              <span class="truncate font-mono">
                {{ log.message }}
              </span>
            </td>
            <td class="w-56 flex items-center overflow-hidden px-4 first:pl-6 last:pr-6 sm:(first:pl-1 last:pr-1)">
              <span class="text-zinc-600 tabular-nums group-hover/row:text-zinc-900">
                {{ dtf.format(log.timestamp) }}
              </span>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>
