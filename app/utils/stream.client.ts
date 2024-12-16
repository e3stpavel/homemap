interface CreateServerSentEventStreamOptions {
  signal: AbortSignal
  withCredentials?: boolean
}

// do not name it like `createEventStream`, I told you
export function* createServerSentEventStream(url: string | URL, { signal, withCredentials }: CreateServerSentEventStreamOptions) {
  signal.throwIfAborted()

  const eventSource = new EventSource(url, {
    withCredentials,
  })

  try {
    while (!signal.aborted) {
      yield {
        message: () => new Promise((resolve, reject) => {
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
        }),
      }
    }
  }
  finally {
    eventSource.close()
  }
}
