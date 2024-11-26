<script setup lang="ts">
const variants = {
  default: {
    base: 'border-zinc-500/10 bg-zinc-50',
    text: 'text-zinc-600',
    dot: 'bg-zinc-400',
  },
  info: {
    base: 'border-blue-700/10 bg-blue-50',
    text: 'text-blue-700',
    dot: 'bg-blue-600',
  },
  success: {
    base: 'border-green-600/20 bg-green-50',
    text: 'text-green-700',
    dot: 'bg-green-600',
  },
  error: {
    base: 'border-red-600/10 bg-red-50',
    text: 'text-red-700',
    dot: 'bg-red-600',
  },
  warning: {
    base: 'border-yellow-600/20 bg-yellow-50',
    text: 'text-yellow-800',
    dot: 'bg-yellow-600',
  },
} satisfies Record<string, Record<'base' | 'text' | 'dot', string>>

interface Props {
  label: string
  variant?: keyof typeof variants
  isDot?: true
}

const { variant = 'default', isDot = false } = defineProps<Props>()
</script>

<template>
  <div
    class="h-5 inline-flex items-center justify-center gap-1 border rounded-full px-2"
    :class="variants[variant].base"
  >
    <div
      v-show="isDot"
      class="size-1 rounded-full"
      :class="variants[variant].dot"
    />
    <span
      class="text-xs font-medium"
      :class="variants[variant].text"
    >
      {{ label }}
    </span>
  </div>
</template>
