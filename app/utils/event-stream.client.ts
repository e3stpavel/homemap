interface CreateEventStreamOptions {
  signal: AbortSignal
  withCredentials?: boolean
}

export async function* createEventStream(url: string | URL, { signal, withCredentials }: CreateEventStreamOptions) {
  signal.throwIfAborted()

  const eventSource = new EventSource(url, {
    withCredentials,
  })

  while (!signal.aborted) {
    try {
      const message = await new Promise((resolve, reject) => {
        eventSource.onerror = (event) => {
          reject(new Error('Unexpected error occurred during streaming', { cause: event }))
        }

        signal.onabort = () => {
          reject(new DOMException('The operation was aborted', 'AbortError'))
        }

        eventSource.onmessage = (event) => {
          if (typeof event.data === 'string') {
            try {
              const message = JSON.parse(event.data)
              resolve(message)
            }
            catch { /* empty */ }
          }
        }
      })

      yield {
        message,
      }
    }
    catch (error) {
      if (!signal.aborted)
        console.error(error)

      break
    }
  }

  eventSource.close()
}
