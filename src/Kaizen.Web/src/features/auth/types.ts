import { z } from "zod";

export const roleUnion = z.union([z.literal("Admin"), z.literal("User")]);

export const userSchema = z.object({
  id: z.string(),
  email: z.string(),
  roles: roleUnion.array(),
});

export const loginSchema = z.object({
  email: z.email(),
  password: z.string(),
});

export type Role = z.infer<typeof roleUnion>;
export type User = z.infer<typeof userSchema>;
export type Login = z.infer<typeof loginSchema>;
