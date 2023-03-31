package de.pdinklag.annotations;

import java.lang.annotation.ElementType;
import java.lang.annotation.Target;

/**
 * Annotates method or constructor parameters that may receive a value of <tt>null</tt>.
 */
@Target({ElementType.PARAMETER})
public @interface Nullable {
}
