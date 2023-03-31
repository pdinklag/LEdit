package de.pdinklag.gui;

/**
 * A text field accepting integer inputs.
 */
public class IntField extends ValueField<Integer> {
    private static final long serialVersionUID = -8746972875299564059L;

    public IntField(int value) {
        super(value);
    }

    public IntField() {
        this(0);
    }

    @Override
    protected Integer parse(String text) {
        try {
            return Integer.valueOf(text);
        } catch (NumberFormatException ex) {
            return null;
        }
    }
}
