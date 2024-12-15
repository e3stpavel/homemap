<script setup lang="ts">
defineOptions({
  inheritAttrs: false,
})

interface Props {
  label: string
  name: string
}

defineProps<Props>()

const id = useId()
const model = defineModel<boolean>()
</script>

<template>
  <div class="inline-flex items-center gap-2">
    <SwitchRoot
      :id="id"
      v-model:checked="model"
      :name="name"
      v-bind="$attrs"
      class="peer h-6 w-10 inline-flex shrink-0 cursor-pointer items-center border-2 border-transparent rounded-md transition-colors disabled:(cursor-not-allowed opacity-60) data-[state=checked]:bg-blue-600 data-[state=unchecked]:bg-zinc-100 focus-visible:(outline-none ring-2 ring-offset-1 ring-blue-500/40)"
    >
      <SwitchThumb
        class="block size-5 rounded bg-white shadow-sm transition-transform duration-300 ease-[cubic-bezier(0.16,1,0.3,1)] data-[state=checked]:translate-x-4 data-[state=unchecked]:translate-x-0 rtl:data-[state=checked]:-translate-x-4"
      />
    </SwitchRoot>
    <slot name="label">
      <label
        :for="id"
        class="text-sm font-medium peer-disabled:(cursor-not-allowed opacity-80)"
      >
        {{ label }}
      </label>
    </slot>
  </div>
</template>
