<script setup lang="ts">
// const projectsStore = useProjectsStore()
// const { deviceLogs } = storeToRefs(projectsStore)

// const stream = projectsStore.streamDeviceLogs()
// stream?.startStreaming()

// // when user exits the page
// onBeforeUnmount(() => stream?.stopStreaming())
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
    <div class="mt-6">
      <DeviceLogsTable />
    </div>
  </div>
</template>
