<script setup lang="ts">
import { useForwardProps, type SliderRootProps } from 'radix-vue'

defineOptions({
  inheritAttrs: false,
})

interface Props extends Omit<SliderRootProps, 'defaultValue' | 'modelValue'> {
  label: string
  name: string
}

const props = defineProps<Props>()
const forward = useForwardProps(props)

const model = defineModel<number>({
  required: true,
})
const value = computed({
  get() {
    return [model.value]
  },
  set(newValue) {
    model.value = newValue[0]
  },
})

const id = useId()
</script>

<template>
  <div>
    <slot name="label">
      <div>
        <label
          :id="id"
          class="block text-sm font-medium"
        >
          {{ label }}
        </label>
      </div>
    </slot>
    <div class="mt-4 space-y-2">
      <slot name="before-slider" />
      <SliderRoot
        v-bind="forward"
        v-model="value"
        class="relative w-full flex touch-none select-none items-center data-[orientation=vertical]:(h-full w-auto flex-col) data-[disabled]:opacity-60"
        :aria-labelledby="id"
      >
        <SliderTrack class="relative grow overflow-hidden rounded-full bg-zinc-100 data-[orientation=horizontal]:(h-2 w-full) data-[orientation=vertical]:(h-full w-2)">
          <SliderRange class="absolute bg-zinc-900 data-[orientation=horizontal]:h-full data-[orientation=vertical]:w-full" />
        </SliderTrack>
        <Tooltip :label="`${model}`">
          <SliderThumb
            class="block size-5 border-2 border-zinc-900 rounded bg-white transition-colors data-[disabled]:cursor-not-allowed focus-visible:(outline-none ring-2 ring-offset-1 ring-blue-500/40)"
            :aria-label="label"
          />
        </Tooltip>
      </SliderRoot>
      <slot name="after-slider" />
    </div>
  </div>
</template>
