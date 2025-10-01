import dayjs from "dayjs";
import { z } from "zod";

export const DATETIME_FORMAT = "D MMMM YYYY [at] h:mma";

export const datetimeSchema = z.iso.datetime({ offset: true });

export function formatDate(date: string | Date) {
  return dayjs(date).format(DATETIME_FORMAT);
}
