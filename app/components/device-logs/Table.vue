<script setup lang="ts">
import type { DeviceLog } from '~/domain/device-log'

const dtf = new Intl.DateTimeFormat('en', {
  timeStyle: 'medium',
  dateStyle: 'medium',
  hour12: false,
})

const deviceLogs = ref<DeviceLog[]>(Array.from({ length: 100 }).map((_, i) => ({
  level: 'warning',
  message: i % 2 === 0 ? 'hello lorem ekeepo jekepep jeowpwp mwlw wl' : 'Lorem, ipsum dolor sit amet consectetur adipisicing elit. Eveniet debitis porro et voluptatum officiis quia accusantium, dolore eaque delectus deserunt animi enim eligendi aut beatae sit laboriosam, rem incidunt. Sint placeat necessitatibus ratione sit, nemo aut. Adipisci minima, consectetur blanditiis corporis cum eum doloremque reiciendis sed quidem suscipit. Rerum ut optio, cum corporis veniam blanditiis porro modi accusamus, asperiores deserunt eum quibusdam id nulla! Itaque, esse. Ut saepe necessitatibus at accusamus earum incidunt dolore dolorum consectetur delectus quam explicabo autem error nesciunt repudiandae dolores, architecto illo repellat labore suscipit sed. Possimus dignissimos quisquam eligendi repudiandae at necessitatibus consequatur facilis quas!',
  timestamp: new Date(1733223648563),
  device: {
    id: 1,
    name: 'testing',
    $type: 'lightbulb',
    createdAt: new Date(1733223648563),
    lastModifiedAt: new Date(1733223648563),
  },
})))

const { list, containerProps, wrapperProps } = useVirtualList(deviceLogs, {
  itemHeight: 40,
})

// testing
// const size = 40
// const listRef = useTemplateRef('list')

// const rowVirtualizer = useWindowVirtualizer({
//   count: 10_000,
//   estimateSize: () => size,
//   overscan: 5,
//   getItemKey: (i) => {
//     const log = deviceLogs.at(i)
//     return `${i}-${log?.level}-${log?.device.id}-${log?.timestamp}`
//   },

//   scrollMargin: listRef.value?.offsetTop ?? 0,
//   paddingEnd: size,

//   // hydration is hard
//   initialRect: { width: 0, height: 720 },
// })

// const virtualRows = computed(() => rowVirtualizer.value.getVirtualItems())
// const totalSize = computed(() => rowVirtualizer.value.getTotalSize())
</script>

<template>
  <div
    class="h-screen-md overflow-auto whitespace-nowrap -mx-6"
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
