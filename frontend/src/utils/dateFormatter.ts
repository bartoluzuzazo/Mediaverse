export class dateFormatter {
  public static formatDate(date: string) {
    return date.replace('T', ' ').slice(0, 16)
  }
}
