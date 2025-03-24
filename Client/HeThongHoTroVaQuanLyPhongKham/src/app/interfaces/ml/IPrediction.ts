import { IMedicine } from "./IMedicine";

export interface IPrediction {
    diagnosis: string;
    treatment: string;
    medicines: IMedicine[];
    warning?: string;
  }