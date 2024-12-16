<script setup lang="ts">
import type { DeviceState } from '~/domain/device-state'
import {
  DeviceStateOnOffSwitch,
  DeviceStateTemperatureSlider,
  DeviceStateLightTemperatureSlider,
  DeviceStateBrightnessSlider,
} from '#components'

const model = defineModel<DeviceState>({
  required: true,
})

const components = {
  isTurnedOn: DeviceStateOnOffSwitch,
  temperature: DeviceStateTemperatureSlider,
  lightTemperature: DeviceStateLightTemperatureSlider,
  brightness: DeviceStateBrightnessSlider,
}
</script>

<template>
  <div class="space-y-4">
    <template
      v-for="(_, key) in model"
      :key="key"
    >
      <div
        v-if="key !== '$type'"
        class="py-2"
      >
        <component
          :is="components[key]"
          v-model="model[key]"
        />
      </div>
    </template>
  </div>
</template>
