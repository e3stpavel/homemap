interface CreateEventStreamOptions {
  signal: AbortSignal
}

interface CreateEventStreamResult {
  data: unknown
}

export const createEventStream = (url: string | URL, { signal }: CreateEventStreamOptions) => {
  signal.throwIfAborted()
  const eventSource = new EventSource(url)

  function close() {
    eventSource.close()
  }

  signal.onabort = () => close()

  eventSource.onerror = (e) => {
    console.error(e)
    close()
  }

  return {
    async *[Symbol.asyncIterator]() {
      while (!signal.aborted) {
        yield new Promise<CreateEventStreamResult>((resolve, reject) => {
          eventSource.onmessage = (event) => {
            if (typeof event.data === 'string') {
              try {
                resolve({
                  data: JSON.parse(event.data),
                })
              }
              catch (error) {
                reject(error)
              }
            }
          }
        })
      }
    },
  }
}
