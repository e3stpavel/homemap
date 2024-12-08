<script setup lang="ts">
const variants = {
  head: 'font-medium',
  data: '',
}

interface Props {
  variant?: keyof typeof variants
}

const { variant = 'data' } = defineProps<Props>()
</script>

<template>
  <component
    :is="variant === 'data' ? 'td' : 'th'"
    class="flex items-center overflow-hidden px-4 first:pl-6 last:pr-6 sm:(first:pl-1 last:pr-1)"
    :class="variants[variant]"
  >
    <slot />
  </component>
</template>
