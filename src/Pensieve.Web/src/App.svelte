<script lang="ts">
  type ApiStatus = 'checking' | 'online' | 'offline'

  let status = $state<ApiStatus>('checking')

  async function checkApi() {
    try {
      const response = await fetch('/api/health')
      status = response.ok ? 'online' : 'offline'
    } catch {
      status = 'offline'
    }
  }

  checkApi()
</script>

<main>
  <h1>Pensieve</h1>
  <p>Save the parts of videos that matter.</p>
  <p>API: {status}</p>
</main>
