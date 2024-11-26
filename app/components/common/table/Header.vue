<script setup lang="ts">
interface Props {
  label: string
}

defineProps<Props>()
</script>

<template>
  <th class="border-b border-r px-4 font-medium last:(border-r-none pr-6) first:pl-6 sm:(first:pl-1 last:pr-1)">
    {{ label }}
  </th>
</template>
