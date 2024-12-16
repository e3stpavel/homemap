<script setup lang="ts">
import { useForwardProps, type SliderRootProps } from 'radix-vue'

defineOptions({
  inheritAttrs: false,
})

interface Props extends Omit<SliderRootProps, 'defaultValue' | 'modelValue'> {
  label: string
  name: string
  leadingIcon?: string
  trailingIcon?: string
}

const props = defineProps<Props>()
const forward = useForwardProps(props)

const id = useId()
const model = defineModel<number>({
  required: true,
})

// this is just to simplify API and not to use `[number]` model
//  because we do not have two thumb sliders in our project
const value = computed({
  get() {
    return [model.value]
  },
  set(newValue) {
    model.value = newValue[0]
  },
})
</script>

<template>
  <div>
    <slot
      :id="id"
      name="label"
      :label="label"
    >
      <div>
        <label
          :id="id"
          class="block text-sm font-medium"
        >
          {{ label }}
        </label>
      </div>
    </slot>
    <div class="mt-4 space-y-4">
      <div class="flex items-center gap-4">
        <span
          v-if="leadingIcon"
          class="text-zinc-500"
        >
          <Icon :name="leadingIcon" />
        </span>
        <SliderRoot
          v-bind="forward"
          v-model="value"
          class="relative w-full flex touch-none select-none items-center data-[orientation=vertical]:(h-full w-auto flex-col) data-[disabled]:opacity-60"
          :aria-labelledby="id"
        >
          <SliderTrack class="relative grow overflow-hidden rounded-full bg-zinc-100 data-[orientation=horizontal]:(h-2 w-full) data-[orientation=vertical]:(h-full w-2)">
            <SliderRange class="absolute bg-blue-600 data-[orientation=horizontal]:h-full data-[orientation=vertical]:w-full" />
          </SliderTrack>
          <Tooltip :label="`${model}`">
            <SliderThumb
              class="block size-5 border-2 border-blue-600 rounded bg-white transition-colors data-[disabled]:cursor-not-allowed focus-visible:(outline-none ring-2 ring-offset-1 ring-blue-500/40)"
              :aria-label="label"
            />
          </Tooltip>
        </SliderRoot>
        <span
          v-if="trailingIcon"
          class="text-zinc-500"
        >
          <Icon :name="trailingIcon" />
        </span>
      </div>
      <slot name="trailing-visuals" />
    </div>
  </div>
</template>
