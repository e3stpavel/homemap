<script setup lang="ts">
import { onWatcherCleanup } from 'vue'

const devicesStore = useDevicesStore()
const deviceStateStore = useDeviceStateStore()

const { currentDeviceId } = storeToRefs(devicesStore)
const { currentDeviceState } = storeToRefs(deviceStateStore)

const isLoading = ref(false)

watch(currentDeviceId, async () => {
  isLoading.value = true
  currentDeviceState.value = undefined

  const abortController = new AbortController()
  const signal = abortController.signal
  onWatcherCleanup(() => abortController.abort())

  try {
    await deviceStateStore.getCurrentDeviceState(signal)
    isLoading.value = false
  }
  catch (error) {
    if (signal.aborted)
      return

    if (error instanceof Error) {
      console.error(error)
    }
  }
})

// using watcher here because native html form does not have any
//  change event for programmatically updated inputs
watchDebounced(
  currentDeviceState,
  async (next, prev) => {
    if (!prev || !next)
      return

    try {
      await deviceStateStore.updateCurrentDeviceState(next)
    }
    catch (error) {
      if (error instanceof Error)
        console.error(error)
    }
  },
  {
    debounce: 1000,
    deep: true,
  },
)
</script>

<template>
  <div>
    <h2 class="text-xl font-semibold tracking-tight">
      Details
    </h2>
    <div class="mt-4">
      <div v-if="currentDeviceId">
        <DeviceState
          v-if="!isLoading && currentDeviceState"
          v-model="currentDeviceState"
        />
        <div v-else>
          <div class="h-7 w-24 animate-pulse rounded bg-zinc-200" />
          <div
            v-for="i in 2"
            :key="i"
            class="mt-4 py-2 space-y-2"
          >
            <div class="h-5 w-48 animate-pulse animate-delay-500 rounded bg-zinc-200" />
            <div class="h-10 w-full animate-pulse animate-delay-200 rounded bg-zinc-200" />
          </div>
        </div>
      </div>
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
    </div>
  </div>
</template>
