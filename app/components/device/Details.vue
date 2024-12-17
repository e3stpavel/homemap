<script setup lang="ts">
import { onWatcherCleanup } from 'vue'
import type { DeviceState } from '~/domain/device-state'
import { useDeviceStateService } from '~/services/device-state'

const { $toast } = useNuxtApp()
const devicesStore = useDevicesStore()
const projectsStore = useProjectsStore()
const deviceStateService = useDeviceStateService()

const currentDeviceState = ref<DeviceState>()
const { currentDeviceId } = storeToRefs(devicesStore)

const isLoading = ref(false)

watchEffect(async () => {
  currentDeviceState.value = undefined

  if (!currentDeviceId.value)
    return

  const abortController = new AbortController()
  const signal = abortController.signal
  onWatcherCleanup(() => abortController.abort())

  isLoading.value = true

  try {
    currentDeviceState.value = await deviceStateService.getDeviceState(currentDeviceId.value, signal)
    isLoading.value = false
  }
  catch (error) {
    if (signal.aborted) {
      isLoading.value = false
      return
    }

    if (error instanceof Error) {
      console.error(error)
    }
  }
})

const handleChange = useDebounceFn(async () => {
  if (!currentDeviceId.value || !currentDeviceState.value)
    return

  try {
    // TODO: we might show some sort if loading spinner when pushing the state
    await deviceStateService.setDeviceState(currentDeviceId.value, currentDeviceState.value)
    $toast.info('Device state was updated!', {
      description: 'This device state has been modified',
      action: {
        label: 'View logs',
        onClick: async () => await navigateTo({
          name: 'projects-id-logs',
          params: { id: projectsStore.currentProject!.id },
        }),
      },
    })
  }
  catch (error) {
    if (error instanceof Error)
      console.error(error)
  }
}, 1500)
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
          @change="handleChange"
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
