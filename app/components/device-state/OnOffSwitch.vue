<script setup lang="ts">
const model = defineModel<boolean>({
  required: true,
})

const label = computed(() =>
  model.value ? 'On' : 'Off',
)
</script>

<template>
  <Switch
    v-model="model"
    :label="label"
    name="isTurnedOn"
  />
</template>
