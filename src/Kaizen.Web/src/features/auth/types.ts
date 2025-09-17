import { z } from "zod";

export const roleUnion = z.union([z.literal("Admin"), z.literal("User")]);

export const userSchema = z.object({
  id: z.string(),
  email: z.string(),
  roles: roleUnion.array(),
});

export type Role = z.infer<typeof roleUnion>;
export type User = z.infer<typeof userSchema>;
