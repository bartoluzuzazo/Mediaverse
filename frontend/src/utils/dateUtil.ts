export class dateUtil {
  public static toUtcIsoString(dateStr: string) {
    const date = new Date(dateStr)
    const iso = date.toISOString()
    return iso.replace('Z', '')
  }
  public static toHumanReadable(dateStr: string) {
    const date = new Date(dateStr + 'Z')
    return date.toLocaleString()
  }
}
