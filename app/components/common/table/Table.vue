<script setup lang="ts">
defineOptions({
  inheritAttrs: false,
})
</script>

<template>
  <div class="overflow-x-auto whitespace-nowrap -mx-6">
    <div class="inline-block min-w-full align-middle sm:px-6">
      <table
        v-bind="$attrs"
        class="min-w-full text-left text-sm"
      >
        <slot />
      </table>
    </div>
  </div>
</template>
