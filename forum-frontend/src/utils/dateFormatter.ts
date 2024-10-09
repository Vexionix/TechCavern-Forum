function formatDateString(dateStr: string): string {
  const trimmedDateStr = dateStr.split(".")[0];
  const dateObj = new Date(trimmedDateStr);

  const year = dateObj.getFullYear().toString();
  const month = String(dateObj.getMonth() + 1).padStart(2, "0");
  const day = String(dateObj.getDate()).padStart(2, "0");

  return `${day}/${month}/${year}`;
}

export default formatDateString;
