<script setup lang="ts">
const emits = defineEmits<{
  change: []
}>()

const model = defineModel<number>({
  required: true,
})

const displayValue = computed(() => {
  return `${model.value}°C`
})
</script>

<template>
  <Slider
    v-model="model"
    label="Temperature"
    name="temperature"
    leading-icon="i-material-symbols-mode-cool"
    trailing-icon="i-material-symbols-mode-heat-outline-rounded"
    :min="18"
    :max="30"
    :step=".5"
    @value-commit="emits('change')"
  >
    <template #label="{ id, label }">
      <div class="flex items-center justify-between gap-4">
        <label
          :id="id"
          class="block text-sm font-medium"
        >
          {{ label }}
        </label>
        <output class="text-sm text-zinc-700 font-medium tabular-nums">
          {{ displayValue }}
        </output>
      </div>
    </template>
  </Slider>
</template>
