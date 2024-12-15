<script setup lang="ts">
import { onWatcherCleanup } from 'vue'

const devicesStore = useDevicesStore()
const deviceStateStore = useDeviceStateStore()

const { currentDeviceId } = storeToRefs(devicesStore)
const { currentDeviceState } = storeToRefs(deviceStateStore)

watch(currentDeviceId, async () => {
  const abortController = new AbortController()
  const signal = abortController.signal
  onWatcherCleanup(() => abortController.abort())

  try {
    await deviceStateStore.getCurrentDeviceState(signal)
  }
  catch (error) {
    if (signal.aborted)
      return

    if (error instanceof Error) {
      console.error(error)
    }
  }
})

const isTurnedOn = ref(false)
</script>

<template>
  <span v-if="currentDeviceId">
    hello {{ currentDeviceId }}
    state {{ currentDeviceState }}
    <Switch
      v-model="isTurnedOn"
      :label="isTurnedOn ? 'On' : 'Off'"
      name="isTurnedOn"
    />
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
