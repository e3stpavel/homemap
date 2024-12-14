<script setup lang="ts">
const devicesStore = useDevicesStore()
const { currentDeviceId } = storeToRefs(devicesStore)
</script>

<template>
  <span v-if="currentDeviceId">
    hello {{ currentDeviceId }}
  </span>
  <div
    v-else
    class="mt-2 border border-2 rounded-lg border-dashed px-4 py-8 hover:border-zinc-300"
  >
    <div class="mx-auto w-fit text-3xl text-zinc-600 sm:text-4xl">
      <Icon name="i-material-symbols-lasso-select-rounded" />
    </div>
    <h3 class="mt-2 text-center text-sm font-semibold leading-6">
      Device not selected
    </h3>
    <p class="text-center text-sm text-zinc-600 leading-6">
      Select a device to start managing it
    </p>
  </div>
</template>
