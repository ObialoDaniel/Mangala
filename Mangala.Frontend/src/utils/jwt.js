export function decodeJwt(token) {
  try {
    const payload = token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/')
    const json = decodeURIComponent(
      atob(payload).split('').map(c => '%' + c.charCodeAt(0).toString(16).padStart(2, '0')).join('')
    )
    const claims = JSON.parse(json)
    return { id: claims.sub, username: claims.name, role: claims.role, exp: claims.exp }
  } catch {
    return null
  }
}