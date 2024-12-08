<script setup lang="ts">
const variants = {
  head: '',
  data: 'odd:bg-zinc-50',
}

interface Props {
  variant?: keyof typeof variants
}

const { variant = 'data' } = defineProps<Props>()
</script>

<template>
  <tr
    class="grid cols-[8rem_14rem_48rem_14rem] h-10 divide-x"
    :class="variants[variant]"
  >
    <slot />
  </tr>
</template>
