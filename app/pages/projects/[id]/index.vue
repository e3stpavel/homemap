<script setup lang="ts">
</script>

<template>
  <div>
    <h1 class="text-3xl font-semibold tracking-tight">
      Overview
    </h1>
    <p class="mt-2 leading-7">
      Manage your devices with just a few clicks
    </p>
    <div class="grid mt-6 sm:block">
      <Dropdown>
        <template #trigger>
          <DropdownTrigger>
            <Button
              as="button"
              type="button"
              label="Add new"
              leading-icon="i-material-symbols-add-2-rounded"
              trailing-icon="i-material-symbols-arrow-drop-down-rounded"
              variant="primary"
            />
          </DropdownTrigger>
        </template>
        <DropdownItem
          as="button"
          label="New receiver"
          icon="i-material-symbols-audio-video-receiver-outline-rounded"
        />
        <DropdownItem
          as="button"
          label="New device"
          icon="i-material-symbols-devices-outline-rounded"
        />
      </Dropdown>
    </div>
    <div class="mt-6 flex flex-col gap-8 xl:(flex-row items-start)">
      <div class="flex-1 overflow-hidden border rounded-lg bg-white p-2 shadow-sm">
        <ReceiverList />
      </div>
      <div class="xl:(sticky top-8 w-96)">
        <DeviceDetails />
      </div>
    </div>
  </div>
</template>
