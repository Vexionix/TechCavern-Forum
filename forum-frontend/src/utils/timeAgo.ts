function timeAgo(dateString: string): string {
  const utcDateString = dateString.endsWith("Z")
    ? dateString
    : dateString + "Z";

  const date = new Date(utcDateString);
  // console.log(date.toLocaleString()); <-- pt edited
  const now = new Date(Date.now());

  const secondsAgo = Math.floor((now.getTime() - date.getTime()) / 1000);

  let interval: number;
  let unit: string;

  if (secondsAgo < 60) {
    interval = secondsAgo;
    unit = "second";
  } else if (secondsAgo < 3600) {
    interval = Math.floor(secondsAgo / 60);
    unit = "minute";
  } else if (secondsAgo < 86400) {
    interval = Math.floor(secondsAgo / 3600);
    unit = "hour";
  } else {
    interval = Math.floor(secondsAgo / 86400);
    unit = "day";
  }

  if (interval > 1) {
    unit += "s";
  }

  if (interval === 0) {
    return "Just now";
  }

  return `${interval} ${unit} ago`;
}

export default timeAgo;
