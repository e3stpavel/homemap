<script setup lang="ts">
const variants = {
  condensed: 'h-8',
  normal: 'h-10',
  spacious: 'h-12',
}

interface Props {
  variant?: keyof typeof variants
}

const { variant = 'normal' } = defineProps<Props>()
</script>

<template>
  <tr
    class="group/row relative hover:bg-blue-50/80"
    :class="variants[variant]"
  >
    <slot />
  </tr>
</template>
