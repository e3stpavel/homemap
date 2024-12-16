<script setup lang="ts">
const model = defineModel<boolean>({
  required: true,
})

const label = computed(() =>
  model.value ? 'On' : 'Off',
)

const emits = defineEmits<{
  change: []
}>()
</script>

<template>
  <Switch
    v-model="model"
    :label="label"
    name="isTurnedOn"
    required
    @update:checked="emits('change')"
  />
</template>
