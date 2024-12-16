<script setup lang="ts">
const model = defineModel<number>({
  required: true,
})

const displayValue = computed(() => {
  return `${model.value}K`
})
</script>

<template>
  <Slider
    v-model="model"
    label="Light temperature"
    name="lightTemperature"
    :min="1500"
    :max="7000"
    :step="300"
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

    <template #trailing-visuals>
      <div
        class="flex items-center justify-between gap-2 text-xs text-zinc-600 font-medium"
        aria-hidden="true"
      >
        <span>Warm</span>
        <span>Neutral</span>
        <span>Cool</span>
        <span>Daylight</span>
      </div>
    </template>
  </Slider>
</template>
