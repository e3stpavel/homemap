<script setup lang="ts">
</script>

<template>
  <NuxtRouteAnnouncer />
  <NuxtLoadingIndicator color="#a1a1aa" />
  <NuxtLayout>
    <NuxtPage />
  </NuxtLayout>
  <!-- TODO: this is more of a quick implementation for demo, replace with headless component later -->
  <Toaster
    position="bottom-right"
    :toast-options="{
      unstyled: true,
      classes: {
        toast: 'font-sans flex flex-wrap gap-y-2 gap-x-4 pl-12 p-4 border rounded-md shadow-md w-[--width] bg-white',
        content: 'grid cols-1 gap-1',
        title: 'text-sm font-medium',
        description: 'text-sm text-zinc-600',
        actionButton: 'm-0 text-sm font-medium text-blue-600 hover:(text-blue-500 underline underline-offset-1)',
        cancelButton: 'm-0 text-sm font-medium text-red-600 underline underline-offset-1 hover:(text-red-500 no-underline)',
        icon: 'absolute top-4 border-t-4 border-t-transparent left-4 text-xl text-zinc-400',
      },
    }"
  >
    <template #info-icon>
      <Icon name="i-material-symbols-info-outline-rounded" />
    </template>
  </Toaster>
</template>

<style>
  html {
    --at-apply: 'text-zinc-900';
  }

  html, body, div#__nuxt {
    height: 100%;
  }
</style>
