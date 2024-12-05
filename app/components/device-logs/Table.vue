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
        v-if="projectStore.deviceLogs.length > 0"
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
