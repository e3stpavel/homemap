<script setup lang="ts">
const projectsStore = useProjectsStore()
const { deviceLogs } = storeToRefs(projectsStore)

const stream = projectsStore.streamDeviceLogs()
stream?.startStreaming()

// when user exits the page
onBeforeUnmount(() => stream?.stopStreaming())

// TODO: make better
const dtf = new Intl.DateTimeFormat('en', {
  timeStyle: 'medium',
  dateStyle: 'medium',
  hour12: false,
})
</script>

<template>
  <div>
    logs
    <div class="grid cols-[auto_auto_1fr_auto] gap-2">
      <div
        v-for="deviceLog in deviceLogs"
        :key="`${deviceLog.level}-${deviceLog.device.id}-${deviceLog.timestamp}`"
        class="grid col-span-full grid-cols-subgrid gap-2"
      >
        <div>
          <span class="h-6 inline-flex items-center border rounded-full bg-zinc-200 px-2 text-sm text-zinc-600">
            {{ deviceLog.level }}
          </span>
        </div>
        <p class="text-sm leading-6">
          {{ `${deviceLog.device.$type}: ${deviceLog.device.name}` }}
        </p>
        <p class="text-sm leading-6">
          {{ deviceLog.message }}
        </p>
        <p class="text-sm text-zinc-700 font-mono">
          {{ dtf.format(deviceLog.timestamp) }}
        </p>
      </div>
    </div>
  </div>
</template>
